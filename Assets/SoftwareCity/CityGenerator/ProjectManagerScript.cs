using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectManagerScript : MonoBehaviour {

    public string filename = "Assets/sample2.json";
    private ContentGenerator contentGenerator;
	private Project _currentProject;

	void Start () {
		contentGenerator = Component.FindObjectOfType<ContentGenerator> ();
      
         _currentProject = new Project(filename);

        Dictionary<BuildingProperty, MetricMapping> mappings = new Dictionary<BuildingProperty, MetricMapping>();

        List<string> metricNames = currentProject.metricNames;

        mappings.Add(BuildingProperty.Height, new MetricMapping(metricNames[7], BuildingProperty.Height, new ValueConverter(0, 1, 0, 1f, false), null));
        mappings.Add(BuildingProperty.Width, new MetricMapping(metricNames[10], BuildingProperty.Width, new ValueConverter(0, 1, 0, 10, false), null));

        ColorGradient col = new ColorGradient(new ColorGradient.ColorPoint(0, new Vector3(1, 0, 0)), new ColorGradient.ColorPoint(1, new Vector3(0, 1, 0)));
        mappings.Add(BuildingProperty.Red, new MetricMapping(metricNames[4], BuildingProperty.Red, new ValueConverter(0, 100, 0, 1, true), col));
        mappings.Add(BuildingProperty.Green, new MetricMapping(metricNames[4], BuildingProperty.Green, new ValueConverter(0, 100, 0, 1, true), col));
        mappings.Add(BuildingProperty.Blue, new MetricMapping(metricNames[4], BuildingProperty.Blue, new ValueConverter(0, 100, 0, 1, true), col));

        OnMappingChanged(mappings);
    }

	void Update() {
		
	}

	public Project currentProject {
		get { return _currentProject; }
	}

	public void OnMappingChanged (Dictionary<BuildingProperty, MetricMapping> mappings) {
		_currentProject.SetMappings (mappings);
		contentGenerator.GenerateContent (_currentProject);
	}
}

public class Project {
	private Dictionary<BuildingProperty, MetricMapping> mappings;
	private Node rootNote;
	private List<MetricStats> metricStatsList;
	private ModelFactory modelFactory;

	public Project(string projectFile) {
		string jsonTxt = System.IO.File.ReadAllText(projectFile);
		modelFactory = new ModelFactory (jsonTxt);
		metricStatsList = modelFactory.GetMetricStats();
	}

	public List<MetricStats> metricStats {
		get {
			return metricStatsList;
		}
	}

	public List<string> metricNames {
		get {
			List<string> result = new List<string> ();
			foreach (MetricStats stat in metricStatsList) {
				result.Add (stat.name);
			}
			result.Sort ();
			return result;
		}
	}

	public void SetMappings(Dictionary<BuildingProperty, MetricMapping> mappings) {
		this.mappings = mappings;
	}

	public Node CreateNodeModel(string pathSeparator) {
		return modelFactory.CreateNodeModel (pathSeparator, mappings);
	}

}

public class ValueConverter {

	private float minInputValue;
	private float maxInputValue;
	private float minOutputValue;
	private float maxOutputValue;

	private bool clamp;

	public ValueConverter(float minInputValue, float maxInputValue, float minOutputValue, float maxOutputValue, bool clamp) {
		this.minInputValue = minInputValue;
		this.maxInputValue = maxInputValue;
		this.minOutputValue = minOutputValue;
		this.maxOutputValue = maxOutputValue;
		this.clamp = clamp;
	}

	public float Convert(float value) {
		float result = (value - minInputValue) / (maxInputValue - minInputValue) * (maxOutputValue - minOutputValue) + minOutputValue;
		if (clamp) {
			return Mathf.Max (Mathf.Min (maxOutputValue, result), minOutputValue);
		} else {
			return result;
		}
	}
}

public enum BuildingProperty {
	Height, Width, Red, Green, Blue
}

public class MetricMapping {
	public string metricName;
	public BuildingProperty buildingProperty;
	public ValueConverter valueConverter;
    public ColorGradient colorGradient;

	public MetricMapping(string metricName, BuildingProperty buildingProperty, ValueConverter valueConverter, ColorGradient colorGradient) {
		this.metricName = metricName;
		this.buildingProperty = buildingProperty;
		this.valueConverter = valueConverter;
        this.colorGradient = colorGradient;
	}

	public float Apply(JSONObject jsonDictionary) {
        if (colorGradient == null)
        {
            return valueConverter.Convert(jsonDictionary.GetField(metricName).n);
        } else
        {
            Vector3 col = colorGradient.GetValue(valueConverter.Convert(jsonDictionary.GetField(metricName).n));
            switch(buildingProperty)
            {
                case BuildingProperty.Red:
                    return col.x;
                case BuildingProperty.Green:
                    return col.y;
                case BuildingProperty.Blue:
                    return col.z;
            }
            return 0;
        }
	}
}

public class MetricStats {
	public string name;
	public float minVal;
	public float maxVal;

	public MetricStats(string name, float minVal, float maxVal) {
		this.name = name;
		this.minVal = minVal;
		this.maxVal = maxVal;
	}
}

public class ModelFactory {

	private string jsonString;

	private JSONObject jsonObject;

	private Node rootNode;

	public ModelFactory(string jsonString) {
		this.jsonString = jsonString;
		ParseJson();
	}

	public Node CreateNodeModel(string pathSeparator, Dictionary<BuildingProperty, MetricMapping> mappings) {
		JSONObject nodesArray = jsonObject.GetField ("nodes");
		JSONObject root = nodesArray.list [0];
		return CreateNodeModel (pathSeparator, "", root, mappings);
	}

	private void ParseJson() {
		jsonObject = new JSONObject (jsonString);
	}

	public List<MetricStats> GetMetricStats() {
		Dictionary<string, MetricStats> metrics = new Dictionary<string, MetricStats> ();
		JSONObject nodesArray = jsonObject.GetField ("nodes");
		JSONObject root = nodesArray.list [0];
		AnalyseMetricModel (root, metrics);
		return new List<MetricStats>(metrics.Values);
	}

	private void AnalyseMetricModel(JSONObject folderOrFile, Dictionary<string, MetricStats> metrics) {
		if (folderOrFile.GetField ("type").str.Equals ("Folder")) {
			List<Node> list = new List<Node> ();
			foreach(JSONObject child in folderOrFile.GetField("children").list) {
				AnalyseMetricModel(child, metrics);
			}
		} else {
			JSONObject attribs = folderOrFile.GetField ("attributes");
			foreach (string attrib in attribs.keys) {
				float val = attribs.GetField (attrib).f;
			
				if (!metrics.ContainsKey(attrib)) {
					MetricStats stats =  new MetricStats (attrib, val, val);
					metrics.Add (attrib, stats);
				} else {
					MetricStats stats = metrics [attrib];
					stats.minVal = Mathf.Min (stats.minVal, val);
					stats.maxVal = Mathf.Max (stats.maxVal, val); 
				}
			}
		}
	}

	public Node CreateNodeModel(string pathSeparator, string path, JSONObject folderOrFile, Dictionary<BuildingProperty, MetricMapping> mappings) {
		if (folderOrFile.GetField ("type").str.Equals ("Folder")) {
			List<Node> list = new List<Node> ();
			foreach(JSONObject child in folderOrFile.GetField("children").list) {
				list.Add (CreateNodeModel (pathSeparator, path + ((path.Length != 0) ? pathSeparator : "") + folderOrFile.GetField("name").str, child, mappings));
			}
			return new StreetNode (list.ToArray());
		} else if (folderOrFile.GetField("type").str.Equals("File")) {
            JSONObject attribs = folderOrFile.GetField ("attributes");
			BuildingNode building = new BuildingNode ();
            building.pathName = path;
            building.name = folderOrFile.GetField("name").str;
            building.heightMappedAttribute = mappings[BuildingProperty.Height].metricName;
            building.groundMappedAttribute = mappings[BuildingProperty.Width].metricName;
            building.colorMappedAttribute = mappings[BuildingProperty.Red].metricName;

            building.height = mappings [BuildingProperty.Height].Apply (attribs);
			building.groundSize = mappings [BuildingProperty.Width].Apply (attribs);

			building.color = new Vector3 (
				mappings[BuildingProperty.Red].Apply(attribs), mappings[BuildingProperty.Green].Apply(attribs), mappings[BuildingProperty.Blue].Apply(attribs)
			);

            foreach(KeyValuePair<string, string> attrib in attribs.ToDictionary())
            {
                building.allAttributes[attrib.Key] = attrib.Value;
            }

			return building;
		}
        return null;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;

//list of JSON file with path extensions
//Only NarrativeManager should be able to use this script
//Take in scene number, output NarrativeEvent - Black Box
//Validation and exception handling

namespace JSONFactory {
	class JSONAssembly {

		private static Dictionary<float, string> _resourceList = new Dictionary<float, string> { 
			{ 1, "/Resources/Event1.json"},
            { 1.1f, "/Resources/Event1-1.json"},
            { 2, "/Resources/Event2.json"},
            { 3, "/Resources/Event3.json"},
            { 4, "/Resources/Event4.json"},
            { 5, "/Resources/Event5.json"},
            { 6, "/Resources/Event6.json"},
            { 7, "/Resources/Event7.json"},
            { 8, "/Resources/Event8.json"},
            { 9, "/Resources/Event9.json"},
            { 10, "/Resources/Event10.json"},
            { 11, "/Resources/Event11.json"},
            { 12, "/Resources/Event12.json"},
            { 13, "/Resources/Event13.json"},
            { 13.1f, "/Resources/Event13-1.json"},
            { 14, "/Resources/Event14.json"},
            { 15, "/Resources/Event15.json"},
            { 16, "/Resources/Event16.json"},
            { -1, "/Resources/ButtonEvents.json"}
        };

		public static NarrativeEvent RunJSONFactoryForScene(float sceneNumber) {
			string resourcePath = keyDictionary (sceneNumber);

			if (IsValidJSON (resourcePath) == true) {
				string jsonString = File.ReadAllText (Application.dataPath + resourcePath);
				NarrativeEvent narrativeEvent = JsonMapper.ToObject<NarrativeEvent> (jsonString);

				return narrativeEvent;
			} else {
				throw new Exception ("The JSON is not valid, please check the schema and file extension.");
			}
		}

        public static TutorialEvent RunJSONFactoryForEvent(float eventNumber)
        {
            string resourcePath = keyDictionary(eventNumber);

            if (IsValidJSON(resourcePath) == true)
            {
                string jsonString = File.ReadAllText(Application.dataPath + resourcePath);
                TutorialEvent inputEvent = JsonMapper.ToObject<TutorialEvent>(jsonString);
                
                return inputEvent;
            }
            else
            {
                throw new Exception("The JSON is not valid, please check the schema and file extension.");
            }
        }

        private static string keyDictionary(float key) {
			string resourcePathResult;

			if (_resourceList.TryGetValue (key, out resourcePathResult)) {
				return _resourceList [key];
			} else {
				throw new Exception ("The scene number you provided is not in the resource list. Please check the JSONFactory namespace.");
			}
		}

		private static bool IsValidJSON(string path) {
			return (Path.GetExtension (path) == ".json") ? true : false;
		}

	}
}

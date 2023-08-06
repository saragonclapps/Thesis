using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using UnityEngine;
using UnityEngine.UI;
using Debug = Logger.Debug;

[CreateAssetMenu(fileName = "TutorialConfig", menuName = "ScriptableObjects/TutorialSetup", order = 1)]
public class TutorialSetup : ScriptableObject {
    public List<TutorialSetupEntry> tutorials;

    public TutorialSetupEntryData Get(string show) {
        foreach (var tutorial in tutorials.Where(tutorial => tutorial.key == show)) {
            return tutorial.value;
        }

        throw new Exception("Tutorial not found: (" + show + ")");
    }

    public void Clean() {
        tutorials = tutorials
            .GroupBy(x => x.key)
            .Select(x => x.First())
            .ToList();
    }
}
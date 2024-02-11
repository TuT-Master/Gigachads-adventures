using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public interface IDataPersistance
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}

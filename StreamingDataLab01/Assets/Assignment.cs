
/*
This RPG data streaming assignment was created by Fernando Restituto.
Pixel RPG characters created by Sean Browning.
*/

using System.Collections.Generic;
using UnityEngine;
using System.IO;




#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

public partial class PartyCharacter
{
    public int classID;

    public int health;
    public int mana;

    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;

}


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1

static public class AssignmentPart1
{
    const int PartyCharacterSaveDataSignifier = 0;
    const int PartyCharacterEquipmentSaveDataSignifier = 1;

    static public void SavePartyButtonPressed()
    {

        using (StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + "Party.txt"))

            foreach (PartyCharacter pc in GameContent.partyCharacters)
            {
                //sw.WriteLine(" L O V E");
                Debug.Log("PC class id == " + pc.classID + "PC health == " + pc.health);
                sw.WriteLine(PartyCharacterSaveDataSignifier + "," + pc.classID + "," + pc.health + "," + pc.mana + "," + pc.strength + "," + pc.agility + "," + pc.wisdom + ",");

                foreach (int equip in pc.equipment)
                {
                    sw.WriteLine(PartyCharacterEquipmentSaveDataSignifier + "," + equip);
                };

            };

    }

    static public void LoadPartyButtonPressed()
    {
        GameContent.partyCharacters.Clear();

        StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + "Party.txt");

        string line;

        while ((line = sr.ReadLine()) != null)
        {
            string[] csv = line.Split(',');
            //Debug.Log(csv[0]);
            int signifier = int.Parse(csv[0]);
            switch (signifier)
            {
                case PartyCharacterSaveDataSignifier:
                    PartyCharacter pc = new PartyCharacter(int.Parse(csv[1]), int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6]));
                    GameContent.partyCharacters.AddLast(pc);
                    break;

                case PartyCharacterEquipmentSaveDataSignifier:
                    GameContent.partyCharacters.Last.Value.equipment.AddLast(int.Parse(csv[1]));
                    break;
            }

        }

        GameContent.RefreshUI();

    }

}


#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/

static public class AssignmentPart2
{
    const int PartyCharacterSaveDataSignifier = 0;
    const int PartyCharacterEquipmentSaveDataSignifier = 1;

    const int LastUsedIndexIndexSignifier = 1;
    const int IndexAndNameSignifier = 2;


    static int lastIndexUsed;
    static List<string> partyNames;
    static LinkedList<NameAndIndex> nameAndIndices;

    const string IndexFilePath = "indices.txt";

    static string LastSelectedName;



    static public void GameStart()
    {
        Debug.Log("Game Start");

        ReadSaveIndexManagementFile();

    }

    static public List<string> GetListOfPartyNames()
    {
        //return new List<string>() {
        //    "sample 1",
        //    "sample 2",
        //    "sample 3"
        //};

        return partyNames;
        GameContent.RefreshUI();
    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {
        GameContent.partyCharacters.Clear();

        int intdexToLoad = -1;
        foreach (NameAndIndex nameAndIndex in nameAndIndices)
        {
            if (nameAndIndex.name == selectedName)
            {
                intdexToLoad = nameAndIndex.index;
                LastSelectedName = selectedName;
            }  
        }
        StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + intdexToLoad + ".txt");
        string line;

        while ((line = sr.ReadLine()) != null)
        {
            Debug.Log(line);
            string[] csv = line.Split(',');
            int signifier = int.Parse(csv[0]);
            if (signifier == PartyCharacterSaveDataSignifier)
            {
                PartyCharacter pc = new PartyCharacter(int.Parse(csv[1]), int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6]));
                GameContent.partyCharacters.AddLast(pc);
            }
            else if (signifier == PartyCharacterEquipmentSaveDataSignifier)
            {
                GameContent.partyCharacters.Last.Value.equipment.AddLast(int.Parse(csv[1]));
            }
        };
        
        Debug.Log("Load " + selectedName);
        //Debug.Log("Load " + intdexToLoad);
        GameContent.RefreshUI();
    }

    static public void SavePartyButtonPressed()
    {
        bool isUniqueName = true;
        foreach (NameAndIndex nameAndIndex in nameAndIndices)
        {
            if (nameAndIndex.name == GameContent.GetPartyNameFromInput())
            {
                SaveParty(Application.dataPath + Path.DirectorySeparatorChar + nameAndIndex.index + ".txt");
                isUniqueName = false;
            }
        }
        if (isUniqueName)
        {
            lastIndexUsed++;
            SaveParty(Application.dataPath + Path.DirectorySeparatorChar + lastIndexUsed + ".txt");
            nameAndIndices.AddLast(new NameAndIndex(lastIndexUsed, GameContent.GetPartyNameFromInput()));
        }
        
        Debug.Log("saving");
        SaveIndexManagementFile();
        ReadSaveIndexManagementFile();
        GameContent.RefreshUI();
    }

    static public void NewPartyButtonPressed()
    {
        Debug.Log("Create new");
        SaveIndexManagementFile();
        GameContent.RefreshUI();
    }

    static public void DeletePartyButtonPressed()
    {
        Debug.Log("DeleteCurrent");
        RemoveIndexManagementFile();
        SaveIndexManagementFile();
        ReadSaveIndexManagementFile();
        GameContent.RefreshUI();
    }

    static public void SaveIndexManagementFile()
    {
        Debug.Log("SaveIndexManagement Funtion has been called");
        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + IndexFilePath);
        sw.WriteLine(LastUsedIndexIndexSignifier + "," + lastIndexUsed);
         //Debug.Log("1," + lastIndexUsed);
        foreach (NameAndIndex nameAndIndex in nameAndIndices)
        {
            sw.WriteLine(IndexAndNameSignifier + "," + nameAndIndex.index + "," + nameAndIndex.name);
        }
        sw.Close();
    }

    static public void ReadSaveIndexManagementFile()
    {
        nameAndIndices = new LinkedList<NameAndIndex>();
        if (File.Exists(Application.dataPath + Path.DirectorySeparatorChar + IndexFilePath))
        {
            StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + IndexFilePath);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                //Debug.Log("line->: "+ line);
                string[] csv = line.Split(',');
                int signifier = int.Parse(csv[0]);
                if (signifier == LastUsedIndexIndexSignifier)
                {
                    lastIndexUsed = int.Parse(csv[1]);
                    Debug.Log("lastIndexUsed =" + lastIndexUsed);
                }
                else if (signifier == IndexAndNameSignifier)
                {
                    nameAndIndices.AddLast(new NameAndIndex(int.Parse(csv[1]), csv[2]));
                }
            }


        }
        partyNames = new List<string>();
        foreach (NameAndIndex nameAndIndex in nameAndIndices)
        {
            partyNames.Add(nameAndIndex.name);
        }

        GameContent.RefreshUI();

    }

    static public void SaveParty(string fileName)
    {
        Debug.Log("SaveParty Funtion  has been called");
        StreamWriter sw = new StreamWriter(fileName);
        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            Debug.Log("PC Class id ==" + pc.classID);
            sw.WriteLine(PartyCharacterSaveDataSignifier + "," + pc.classID + "," + pc.health
                + "," + pc.mana
                + "," + pc.strength
                + "," + pc.agility
                + "," + pc.wisdom);
            foreach (int equip in pc.equipment)
            {
                sw.WriteLine(PartyCharacterEquipmentSaveDataSignifier + "," + equip);
            }
        }
        sw.Close();
    }


    static public void RemoveIndexManagementFile()
    {
        //remove  the NameAnDIndex object
        NameAndIndex tempObject = new NameAndIndex(90,"90");
        

        foreach (NameAndIndex nameAndIndex in nameAndIndices)
        {

            if (nameAndIndex.name == LastSelectedName)
            {
                tempObject = nameAndIndex;
                break;
            }
        }
        nameAndIndices.Remove(tempObject);

        ///remove the file
        File.Delete(Application.dataPath + Path.DirectorySeparatorChar + tempObject.index + ".txt");
        GameContent.RefreshUI();

    }


    static public void SendOnScreenPartyToServerForSharing(NetWorkClient networkClient)
    {
        LinkedList<string> data = new LinkedList<string>();
      

        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            Debug.Log("PC Class id ==" + pc.classID);
            data.AddLast(PartyCharacterSaveDataSignifier + "," + pc.classID + "," + pc.health
                + "," + pc.mana
                + "," + pc.strength
                + "," + pc.agility
                + "," + pc.wisdom);
            foreach (int equip in pc.equipment)
            {
                data.AddLast(PartyCharacterEquipmentSaveDataSignifier + "," + equip);
            }
        }

        networkClient.SendMessageToHost(ClientToServerSignifiers.PartyTransferDataStart + "" );
        foreach (string d in data) 
        {
            networkClient.SendMessageToHost(ClientToServerSignifiers.PartyTransferData+ "," +d);
        }
        networkClient.SendMessageToHost(ClientToServerSignifiers.PartyTransferDataEnd + "");
    }


    static public  void LoadPartyFromSharedFriend(LinkedList<string> data ) 
    {
        GameContent.partyCharacters.Clear();
        foreach (string line in data) 
        {
            Debug.Log(line);
            string[] csv = line.Split(',');
            int signifier = int.Parse(csv[1]);
            if (signifier == PartyCharacterSaveDataSignifier)
            {
                PartyCharacter pc = new PartyCharacter(int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6]), int.Parse(csv[7]));
                GameContent.partyCharacters.AddLast(pc);
            }
            else if (signifier == PartyCharacterEquipmentSaveDataSignifier)
            {
                GameContent.partyCharacters.Last.Value.equipment.AddLast(int.Parse(csv[2]));
            }
        };
        GameContent.RefreshUI();
    }


    public class NameAndIndex
    {
        public string name;
        public int index;

        public NameAndIndex(int Index, string Name)
        {
            name = Name;
            index = Index;
        }
        public string GetName
        {
            get { return name; }
        }
        public int  GetIndex
        {
            get { return index; }
        }
    }

}

#endregion
//#region Assignment Part 2




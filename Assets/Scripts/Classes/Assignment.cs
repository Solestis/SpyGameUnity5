using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container class for the Assignment object. Holds the data and states for an Assignment within the game.
/// </summary>
public class Assignment {

    #region Assignment states
    public static int STATE_NEW = 0;
    public static int STATE_ONGOING = 1;
    public static int STATE_FINISHED = 2; 
    #endregion

	public ExitGames.Client.Photon.Hashtable typeData = new ExitGames.Client.Photon.Hashtable ();
	public int assignmentState { get; set; }
	public int sequenceNumber { get; set; }
	public string assignmentType { get; private set; }
	public string description;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="Assignment"/> class. Retrieves and deletes the target location and description from data. The remainder of data is saved as typeData.
    /// </summary>
    /// <param name="sequenceN">The sequence number of the assignment.</param>
    /// <param name="type">The type of assignment, must correspond with and assignment scene.</param>
    /// <param name="data">Hashtable containing the rest of the assignment data, including type specific data.</param>
    public Assignment(int sequenceN, string type, ExitGames.Client.Photon.Hashtable data = null)
    {
        assignmentType = type;
        sequenceNumber = sequenceN;

        assignmentState = Assignment.STATE_NEW;

        if (data != null)
        {
            object value;
            if (data.TryGetValue("missionDescription", out value))
            {
                description = (string)value;
                data.Remove("missionDescription");
            }
            typeData = data;
        }
    } 
    #endregion


    #region Public Methods    
    /// <summary>
    /// Sets the assignmentState of the mission to ONGOING.
    /// </summary>
    public void Start()
    {
        assignmentState = Assignment.STATE_ONGOING;
    }

    /// <summary>
    /// Sets the assignmentState of the mission to FINISHED, loads the "Mission" scene.
    /// </summary>
    public void FinishAssignment()
    {
        assignmentState = Assignment.STATE_FINISHED;
    }

    /// <summary>
    /// Serializes all non static variables to a ExitGames.Client.Photon.Hashtable which can be sent over the Photon network.
    /// </summary>
    /// <returns>ExitGames.Client.Photon.Hashtable</returns>
    public ExitGames.Client.Photon.Hashtable ToHashtable()
    {
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("sequenceNumber", sequenceNumber);
        Debug.Log("Added sequenceNumber " + sequenceNumber + " to Hashtable");
        h.Add("assignmentType", assignmentType);
        Debug.Log("Added assignmentType " + assignmentType + " to Hashtable");
        h.Add("missionDescription", description);
        Debug.Log("Added missionDescription " + description + " to Hashtable");
        foreach (DictionaryEntry entry in typeData)
        {
            h.Add(entry.Key, entry.Value);
        }
        Debug.Log("Added typeData " + typeData + " to Hashtable");
        return h;
    } 
    #endregion
}

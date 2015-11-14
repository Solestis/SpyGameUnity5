using UnityEngine;
using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

/// <summary>
/// Container class for the Mission object. Contains all assignments for the mission, tracks the active assignment and contains mission states.
/// </summary>
public class Mission {

    #region Mission states
    public const int STATE_NEW = 0;
    public const int STATE_ONGOING = 1;
    public const int STATE_FINISHED = 2; 
    #endregion

	public int missionState;
	public DateTime expirationDate;
	public int missionDuration; //In hours
    public string title;
    public string description;
	public Assignment activeAssignment { get; private set; }

    private Dictionary<int, Assignment> assignments;

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="Mission"/> class.
    /// </summary>
    /// <param name="expiration">The time in hours before the mission expires after recieving it.</param>
    /// <param name="duration">The time available to complete the mission once started.</param>
    /// <param name="startAssignments">Optional Dictionary of assignments to instantly fill the mission.</param>
    public Mission(DateTime expiration, int duration = 2, Dictionary<int, Assignment> startAssignments = null)
    {
        assignments = startAssignments;
        missionDuration = duration;
        expirationDate = expiration;
        missionState = Mission.STATE_NEW;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mission"/> class.
    /// </summary>
    /// <param name="missionData">Hashtable of mission data including expirationDate, missionDuration, assignmentCount and assignments as Hashtables themselves.</param>
    public Mission(Hashtable missionData)
    {
        object value;
        if (missionData.TryGetValue("expirationDate", out value))
        {
            expirationDate = DateTime.FromBinary((Int64)value);
        }
        else
        {
            Debug.Log("No expirationDate found");
        }

        if (missionData.TryGetValue("missionDuration", out value))
        {
            missionDuration = (int)value;
        }
        else
        {
            Debug.Log("No missionDuration found");
        }

        if (missionData.TryGetValue("assignmentCount", out value))
        {
            int assignmentCount = (int)value;
            assignments = new Dictionary<int, Assignment>();
            for (int i = 1; i <= assignmentCount; i++)
            {
                Hashtable assignmentData = null;
                string type = "None";
                int seqn = 0;
                if (missionData.TryGetValue(i, out value))
                {
                    assignmentData = (Hashtable)value;
                }
                if (assignmentData.TryGetValue("assignmentType", out value))
                {
                    type = (string)value;
                    assignmentData.Remove("assignmentType");
                }
                if (assignmentData.TryGetValue("sequenceNumber", out value))
                {
                    seqn = (int)value;
                    assignmentData.Remove("sequenceNumber");
                }
                Assignment a = new Assignment(seqn, type, assignmentData);
                assignments.Add(seqn, a);
            }
            Debug.Log("new mission made containing " + assignments.Count + " assignments");
        }
        else
        {
            Debug.Log("No assignmentCount found, not loading any assignments.");
        }
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Set mission data without using the constructor.
    /// </summary>
    /// <param name="missionTitle">The display name of the mission.</param>
    /// <param name="missionDescription">The description of the mission.</param>
    /// <param name="duration">The amount of time players have to complete the mission.</param>
    public void SetMissionData(string missionTitle, string missionDescription, int duration = 0)
    {
        title = missionTitle;
        description = missionDescription;
        if (missionDuration != 0)
        {
            missionDuration = duration;
        }
    }

    /// <summary>
    /// Progress the mission to the next assignment, moves the target to the new location. If no more assignments are left, sets the missionstate to FINISHED.
    /// </summary>
    public void NextAssignment()
    {
        if (activeAssignment == null)
        {
            activeAssignment = assignments[1];
            missionState = Mission.STATE_ONGOING;
        }
        else
        {
            int nextAssignmentNumber = activeAssignment.sequenceNumber + 1;
            if (nextAssignmentNumber <= assignments.Count)
            {
                activeAssignment = assignments[nextAssignmentNumber];
            }
            else
            {
                missionState = Mission.STATE_FINISHED;
            }
        }
    }

    /// <summary>
    /// Serializes all non static variables to a ExitGames.Client.Photon.Hashtable which can be sent over the Photon network.
    /// </summary>
    /// <returns>ExitGames.Client.Photon.Hashtable</returns>
    public Hashtable ToHashtable()
    {
        Hashtable h = new Hashtable();
        h.Add("expirationDate", expirationDate.ToBinary());
        h.Add("missionTitle", title);
        h.Add("missionDescription", description);
        h.Add("missionDuration", missionDuration);
        h.Add("assignmentCount", assignments.Count);
        Debug.Log("mission to hashtable without assignments: " + h);
        foreach (KeyValuePair<int, Assignment> entry in assignments)
        {
            Hashtable ah = entry.Value.ToHashtable();
            h.Add(entry.Key, ah);
        }
        Debug.Log("mission to hashtable: " + h);
        return h;
    } 
    #endregion
}

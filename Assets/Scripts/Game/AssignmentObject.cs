using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Script used in the MissionMaker. Draggable representation of the different assignments in the game.
/// </summary>
public class AssignmentObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    #region Linked in Editor
    public Transform menuParent;
    public GameObject targetParent;
    public MissionMaker missionMaker;
    public WIAssignmentMaker assignmentMaker;
    public string assignmentType;
    #endregion

	public Assignment assignment { get; private set; }

	private RectTransform rectTransform;
	private Transform parentBeforeDrag;
	private int initialSiblingIndex;


    #region Monobehaviour Event Handlers
    /// <summary>
    /// On Monobehaviour Start, get and store the RectTransform component of this GameObject and gets the Sibling Index of this GameObject.
    /// </summary>
    public void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        initialSiblingIndex = this.transform.GetSiblingIndex();
    } 
    #endregion

    #region UnityUI Event Handlers
    /// <summary>
    /// On UnityUI Begin Drag, stores the previous parent transform, and sets the new parent to be the mainCanvas.
    /// </summary>
    /// <param name="e">PointerEventData provided by the UnityUI Eventsystem.</param>
    public void OnBeginDrag(PointerEventData e)
    {
        parentBeforeDrag = this.transform.parent.transform;
        rectTransform.SetParent(UICanvas.canvas.transform);
    }

    /// <summary>
    /// On UnityUI Drag, sets the position of the GameObject to be at the position of the pointer.
    /// </summary>
    /// <param name="e">PointerEventData provided by the UnityUI Eventsystem.</param>
    public void OnDrag(PointerEventData e)
    {
        Vector3 dragPosition = new Vector3(e.position.x, e.position.y, 0);
        this.gameObject.transform.position = dragPosition;
    }

    /// <summary>
    /// On UnityUI End Drag, determines where the drag ended, and acts accordingly. If drag ended on the targetParent which wasnt also its previous parent, creates a copy of itself to remain there. This GameObject then moves back to its original position.
    /// </summary>
    /// <param name="e"></param>
    public void OnEndDrag(PointerEventData e)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = e.position;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        List<GameObject> resultGameObjects = new List<GameObject>();
        foreach (RaycastResult result in raycastResults)
        {
            resultGameObjects.Add(result.gameObject);
        }

        if (resultGameObjects.Contains(targetParent))
        {
            if (parentBeforeDrag == menuParent)
            {
                GameObject cloneObject = Instantiate(this.gameObject) as GameObject;
                cloneObject.GetComponent<RectTransform>().SetParent(targetParent.transform);
                cloneObject.transform.localScale = Vector3.one;
                cloneObject.GetComponent<Button>().interactable = true;

                assignmentMaker.ConfigureNewAssignment(assignmentType, cloneObject.GetComponent<AssignmentObject>());

                rectTransform.SetParent(menuParent);
                this.transform.SetSiblingIndex(initialSiblingIndex);
            }
            else
            {
                rectTransform.SetParent(targetParent.transform);
            }
        }
        else if (resultGameObjects.Contains(targetParent) == false && parentBeforeDrag != menuParent)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            rectTransform.SetParent(menuParent);
            this.transform.SetSiblingIndex(initialSiblingIndex);
        }
    } 
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds an assignment to this UI object.
    /// </summary>
    /// <param name="assignmentToAdd"></param>
    public void AddAssignment(Assignment assignmentToAdd)
    {
        assignment = assignmentToAdd;
    } 
    #endregion
}

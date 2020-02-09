
Namespace FluentAccess

  Public Interface ICardAccessContext

    'Identity
    ReadOnly Property Id As String
    Function Exists(Optional includeArchived As Boolean = False) As Boolean
    Function IsArchived() As Boolean

    'Editing
    Function Load() As IPersistentTrelloCard
    Sub LoadInto(target As ITrelloCardContent)
    Sub SaveContentFrom(contentSource As ITrelloCardContent)

    'Archive
    Sub Archive()
    Sub Unarchive()

    'Move
    Sub MoveTo(targetList As IListAccessContext)
    Sub MoveTo(targetListId As String)
    Sub MoveTo(targetList As IPersistentTrelloList)

    'Principal/Dependent-Navigation
    ReadOnly Property List() As IListAccessContext
    ReadOnly Property Board() As IBoardAccessContext

    'Referrer-Navigation ->  Labels
    Function AssignedLabels() As ILabelsAccessContext

    Function IsAssignedToLabel(label As ILabelAccessContext) As Boolean
    Function IsAssignedToLabel(labelId As String) As Boolean
    Function IsAssignedToLabel(label As IPersistentTrelloLabel) As Boolean

    Sub AssignLabel(label As ILabelAccessContext)
    Sub AssignLabel(labelId As String)
    Sub AssignLabel(label As IPersistentTrelloLabel)

    Sub UnassignLabel(label As ILabelAccessContext)
    Sub UnassignLabel(labelId As String)
    Sub UnassignLabel(label As IPersistentTrelloLabel)

    'Referrer-Navigation -> Members

    Function AssignedMembers() As IMembersAccessContext

    Function IsAssignedToMember(member As IMemberAccessContext) As Boolean
    Function IsAssignedToMember(memberId As String) As Boolean
    Function IsAssignedToMember(member As IPersistentTrelloMember) As Boolean

    Sub AssignMember(member As IMemberAccessContext)
    Sub AssignMember(memberId As String)
    Sub AssignMember(member As IPersistentTrelloMember)

    Sub UnassignMember(member As IMemberAccessContext)
    Sub UnassignMember(memberId As String)
    Sub UnassignMember(member As IPersistentTrelloMember)

  End Interface

End Namespace

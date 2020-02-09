
Namespace FluentAccess

  Public Interface IBoardAccessContext

    'Identity
    ReadOnly Property Id As String
    Function Exists(Optional includeArchived As Boolean = False) As Boolean
    Function IsArchived() As Boolean

    'Editing
    Function Load() As IPersistentTrelloBoard
    Sub LoadInto(target As ITrelloBoardContent)
    Sub SaveContentFrom(contentSource As ITrelloBoardContent)

    'Archive
    Sub Archive()

    'Principal/Dependent-Navigation

    ''' <summary> over all lists </summary>
    ReadOnly Property Cards As ICardsAccessContext

    ReadOnly Property Lists As IListContainerAccessContext

    ReadOnly Property Labels As ILabelContainerAccessContext

    'Referrer-Navigation -> Opened by Member
    Function AccessingMembers() As IMembersAccessContext

  End Interface

End Namespace

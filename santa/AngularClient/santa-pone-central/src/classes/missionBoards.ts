export class BoardEntry
{
  boardEntryID: string;
  entryType: EntryType = new EntryType;
  postNumber: number;
  postDescription: string;
}
export class EntryType
{
  entryTypeID: string;
  entryTypeName: string;
  entryTypeDescription: string;
  adminOnly: boolean;
}

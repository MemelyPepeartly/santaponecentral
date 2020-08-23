export class BoardEntry
{
  boardEntryID: string;
  entryType: EntryType = new EntryType;
  dateTimeEntered: Date;
  threadNumber: number;
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

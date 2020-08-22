import { Component, OnInit, Input } from '@angular/core';
import { BoardEntry, EntryType } from 'src/classes/missionBoards';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-mission-board-table',
  templateUrl: './mission-board-table.component.html',
  styleUrls: ['./mission-board-table.component.css']
})
export class MissionBoardTableComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) { }

  @Input() boardEntries: Array<BoardEntry> = [];
  @Input() entryType: EntryType = new EntryType();
  @Input() allowForm: boolean;

  public boardEntryFormGroup: FormGroup;
  get boardEntryFormControls()
  {
    return this.boardEntryFormGroup.controls;
  }

  public showFormFields: boolean = false;
  public postingEntry: boolean = false;

  columns: string[] = ["number", "description", "type"];

  ngOnInit(): void {
    this.boardEntryFormGroup = this.formBuilder.group({
      postNumber: ['', [Validators.required, Validators.maxLength(10), Validators.pattern("^[0-9]*$")]],
      postDescription: ['', [Validators.required, Validators.maxLength(100)]],
    });
  }
  submitBoardEntry()
  {

  }
}

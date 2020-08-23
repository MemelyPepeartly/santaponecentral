import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BoardEntry, EntryType } from 'src/classes/missionBoards';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MissionBoardAPIService } from 'src/app/services/santaApiService.service';
import { NewBoardEntryResponse } from 'src/classes/responseTypes';

@Component({
  selector: 'app-mission-board-table',
  templateUrl: './mission-board-table.component.html',
  styleUrls: ['./mission-board-table.component.css']
})
export class MissionBoardTableComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    private missionBoardAPIService: MissionBoardAPIService) { }

  @Input() boardEntries: Array<BoardEntry> = [];
  @Input() entryType: EntryType = new EntryType();
  @Input() allowForm: boolean;

  @Output() formPostedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public boardEntryFormGroup: FormGroup;
  get boardEntryFormControls()
  {
    return this.boardEntryFormGroup.controls;
  }

  public showFormFields: boolean = false;
  public postingEntry: boolean = false;
  public postSuccess: boolean = false;

  columns: string[] = ["number", "description", "type"];

  ngOnInit(): void {
    this.boardEntryFormGroup = this.formBuilder.group({
      postNumber: ['', [Validators.required, Validators.maxLength(10), Validators.pattern("^[0-9]*$")]],
      postDescription: ['', [Validators.required, Validators.maxLength(100)]],
    });
  }
  async submitBoardEntry()
  {
    this.postingEntry = true;
    this.postSuccess = true;

    let response: NewBoardEntryResponse = new NewBoardEntryResponse();

    response.entryTypeID = this.entryType.entryTypeID;

    response.postNumber = Number(this.boardEntryFormControls.postNumber.value);
    response.postDescription = this.boardEntryFormControls.postDescription.value;

    await this.missionBoardAPIService.postNewBoardEntry(response).toPromise().catch((err) => {console.log("Something went wrong: " + err); this.postSuccess = false});
    this.formPostedEvent.emit(this.postSuccess);

    this.postingEntry = false;
  }
}

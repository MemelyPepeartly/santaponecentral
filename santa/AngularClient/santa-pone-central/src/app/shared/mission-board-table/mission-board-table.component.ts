import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BoardEntry, EntryType } from 'src/classes/missionBoards';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MissionBoardAPIService } from 'src/app/services/santaApiService.service';
import { NewBoardEntryResponse } from 'src/classes/responseTypes';
import { MissionBoardService } from 'src/app/services/MissionBoardService.service';

@Component({
  selector: 'app-mission-board-table',
  templateUrl: './mission-board-table.component.html',
  styleUrls: ['./mission-board-table.component.css']
})
export class MissionBoardTableComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    private missionBoardAPIService: MissionBoardAPIService) { }

  @Input() boardEntries: Array<BoardEntry> = [];
  @Input() allPostNumbers: Array<number>= []
  @Input() entryType: EntryType = new EntryType();
  @Input() allowForm: boolean;

  @Output() formPostedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public boardEntryFormGroup: FormGroup;
  public editPostFormGroup: FormGroup;
  get boardEntryFormControls()
  {
    return this.boardEntryFormGroup.controls;
  }

  public showFormFields: boolean = false;
  public postingEntry: boolean = false;
  public postSuccess: boolean = false;
  get postNumberExists(): boolean
  {
    return this.allPostNumbers.some((postNumber: number) => {
      return postNumber == Number(this.boardEntryFormControls.postNumber.value);
    });
  }
  get threadNumberExists(): boolean
  {
    return this.allPostNumbers.some((postNumber: number) => {
      return postNumber == Number(this.boardEntryFormControls.postNumber.value);
    });
  }

  viewerColumns: string[] = ["threadNumber", "postNumber", "description"];
  helperColumns: string[] = ["threadNumber", "postNumber", "description", "actions"];

  ngOnInit() {
    this.editPostFormGroup = this.formBuilder.group({});
    this.addControls();
    this.boardEntryFormGroup = this.formBuilder.group({
      threadNumber: ['', [Validators.required, Validators.maxLength(10), Validators.pattern("^[0-9]*$")]],
      postNumber: ['', [Validators.required, Validators.maxLength(10), Validators.pattern("^[0-9]*$")]],
      postDescription: ['', [Validators.required, Validators.maxLength(100)]],
    });
  }
  private addControls()
  {
    this.boardEntries.forEach((entry: BoardEntry) => {
      this.editPostFormGroup.addControl(this.getThreadNumberFormGroupSignature(entry), new FormControl(entry.threadNumber, [Validators.required, Validators.maxLength(10), Validators.pattern("^[0-9]*$")]));
      this.editPostFormGroup.addControl(this.getPostNumberFormGroupSignature(entry), new FormControl(entry.postNumber, [Validators.required, Validators.maxLength(10), Validators.pattern("^[0-9]*$")]));
      this.editPostFormGroup.addControl(this.getDescriptionFormGroupSignature(entry), new FormControl(entry.postDescription, [Validators.required, Validators.maxLength(10), Validators.pattern("^[0-9]*$")]));
    });
  }
  public getDescriptionFormGroupSignature(entry: BoardEntry) : string
  {
    return entry.boardEntryID + "DESCRIPTION";
  }
  public getThreadNumberFormGroupSignature(entry: BoardEntry) : string
  {
    return entry.boardEntryID + "THREAD";
  }
  public getPostNumberFormGroupSignature(entry: BoardEntry) : string
  {
    return entry.boardEntryID + "POST";
  }
  async submitBoardEntry()
  {
    this.postingEntry = true;
    this.postSuccess = true;

    let response: NewBoardEntryResponse = new NewBoardEntryResponse();

    response.entryTypeID = this.entryType.entryTypeID;

    response.threadNumber = Number(this.boardEntryFormControls.threadNumber.value);
    response.postNumber = Number(this.boardEntryFormControls.postNumber.value);
    response.postDescription = this.boardEntryFormControls.postDescription.value;

    await this.missionBoardAPIService.postNewBoardEntry(response).toPromise().catch((err) => {console.log("Something went wrong: " + err); this.postSuccess = false});
    this.formPostedEvent.emit(this.postSuccess);
    this.boardEntryFormGroup.reset();

    this.postingEntry = false;
  }
  async deleteEntry(entry: BoardEntry)
  {

  }
  async submitEntryEdits(entry: BoardEntry)
  {

  }
  async goToThreadLink(threadNumber: number)
  {
    let url: string = "https://boards.4channel.org/mlp/thread/" + threadNumber
    window.open(url, "_blank");
  }
  async goToPostLink(threadNumber: number, postNumber: number)
  {
    let url: string = "https://boards.4channel.org/mlp/thread/" + threadNumber + "#p" + postNumber
    window.open(url, "_blank");
  }
  public areNumbersValid() : boolean
  {
    if(Number(this.boardEntryFormControls.threadNumber.value) == NaN || Number(this.boardEntryFormControls.postNumber.value) == NaN)
    {
      return false;
    }
    else
    {
      // If thread exists, but post number doesnt, it is valid
      if(this.threadNumberExists && !this.postNumberExists)
      {
        return true;
      }
      // else if the thread number exists and the post numer exists, it is invalid
      else if(this.threadNumberExists && this.postNumberExists)
      {
        return false;
      }
    }
  }
  public getThreadNumberFormControl(entry: BoardEntry)
  {
    return this.editPostFormGroup.get(entry.threadNumber.toString()) as FormControl;
  }
  public getPostNumberFormControl(entry: BoardEntry)
  {
    return this.editPostFormGroup.get(entry.postNumber.toString()) as FormControl;
  }
  public getDescriptionFormControl(entry: BoardEntry)
  {
    return this.editPostFormGroup.get(this.getDescriptionFormGroupSignature(entry)) as FormControl;
  }
}

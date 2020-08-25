import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BoardEntry, EntryType } from 'src/classes/missionBoards';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MissionBoardAPIService } from 'src/app/services/santaApiService.service';
import { NewBoardEntryResponse, EditBoardEntryThreadNumberResponse, EditBoardEntryPostNumberResponse, EditBoardEntryPostDescriptionResponse } from 'src/classes/responseTypes';
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

  @Output() hardRefreshEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() softRefreshEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public boardEntryFormGroup: FormGroup;
  public editPostFormGroup: FormGroup;
  get boardEntryFormControls()
  {
    return this.boardEntryFormGroup.controls;
  }

  public showFormFields: boolean = false;
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

  public deletingEntry: boolean;
  public editingEntry: boolean;
  public postingEntry: boolean;

  public postSuccess: boolean;
  public totalPutSuccess: boolean;
  public deleteSuccess: boolean;

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

    await this.missionBoardAPIService.postNewBoardEntry(response).toPromise().catch((err) => {
      console.log("Something went wrong submitting the entry");
      console.log(err);
      this.postSuccess = false
    });
    this.hardRefreshEvent.emit(this.postSuccess);
    this.boardEntryFormGroup.reset();

    this.postingEntry = false;
  }
  async deleteEntry(entry: BoardEntry)
  {
    this.deletingEntry = true;
    this.deleteSuccess = true;

    await this.missionBoardAPIService.deleteBoardEntryByID(entry.boardEntryID).toPromise().catch((err) => {
      console.log("Something went wrong deleting the entry");
      console.log(err);
      this.deleteSuccess = false
    });
    this.hardRefreshEvent.emit(this.deleteSuccess);

    this.deletingEntry = false;
  }
  async submitEntryEdits(entry: BoardEntry)
  {
    this.editingEntry = true;
    let partialSuccess: boolean = false;

    let threadSuccess: boolean = true;
    let postSuccess: boolean = true;
    let descriptionSuccess: boolean = true;



    // If thread number was changed
    if(entry.threadNumber != this.editPostFormGroup.get(this.getThreadNumberFormGroupSignature(entry)).value)
    {
      let threadResponse: EditBoardEntryThreadNumberResponse = new EditBoardEntryThreadNumberResponse();
      threadResponse.threadNumber = Number(this.editPostFormGroup.get(this.getThreadNumberFormGroupSignature(entry)).value);

      await this.missionBoardAPIService.putBoardEntryThreadNumber(entry.boardEntryID, threadResponse).toPromise().catch((err) => {
        console.log("Something went wrong changing the thread number");
        console.log(err);
        threadSuccess = false
      });
    }
    // If post number was changed
    if(entry.postNumber != this.editPostFormGroup.get(this.getPostNumberFormGroupSignature(entry)).value)
    {
      let postResponse: EditBoardEntryPostNumberResponse = new EditBoardEntryPostNumberResponse();
      postResponse.postNumber = Number(this.editPostFormGroup.get(this.getPostNumberFormGroupSignature(entry)).value);

      await this.missionBoardAPIService.putBoardEntryPostNumber(entry.boardEntryID, postResponse).toPromise().catch((err) => {
        console.log("Something went wrong changing the post number");
        console.log(err);
        postSuccess = false
      });
    }
    // If thread number was changed
    if(entry.postDescription != this.editPostFormGroup.get(this.getDescriptionFormGroupSignature(entry)).value)
    {
      let descriptionResponse: EditBoardEntryPostDescriptionResponse = new EditBoardEntryPostDescriptionResponse();
      descriptionResponse.postDescription = this.editPostFormGroup.get(this.getDescriptionFormGroupSignature(entry)).value;

      await this.missionBoardAPIService.putBoardEntryPostDescription(entry.boardEntryID, descriptionResponse).toPromise().catch((err) => {
        console.log("Something went wrong changing the entry description");
        console.log(err);
        descriptionSuccess = false
      });
    }

    if(!threadSuccess || !postSuccess || !descriptionSuccess)
    {
      this.totalPutSuccess = false;
    }
    if(threadSuccess || postSuccess || descriptionSuccess)
    {
      partialSuccess = true;
      this.softRefreshEvent.emit(partialSuccess);
    }
    this.boardEntries.find((item: BoardEntry) => {return item.boardEntryID == entry.boardEntryID}).editing = false;

    this.editingEntry = false;
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

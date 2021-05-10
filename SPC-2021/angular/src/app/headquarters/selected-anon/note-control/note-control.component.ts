import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MapService } from 'src/app/services/mapper.service';
import { SantaApiDeleteService, SantaApiGetService, SantaApiPostService, SantaApiPutService } from 'src/app/services/santa-api.service';
import { Note } from 'src/classes/note';
import { EditNoteResponse, NewNoteResponse } from 'src/classes/responseTypes';

@Component({
  selector: 'app-note-control',
  templateUrl: './note-control.component.html',
  styleUrls: ['./note-control.component.css']
})
export class NoteControlComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    public mapper: MapService,
    public SantaApiGet: SantaApiGetService,
    public SantaApiPut: SantaApiPutService,
    public SantaApiPost: SantaApiPostService,
    public SantaApiDelete: SantaApiDeleteService) { }

  @Input() clientID: string;
  @Input() notes: Array<Note> = [];
  @Input() infoOnly: boolean = false;
  @Input() hasMargin: boolean = false;
  @Input() createNewNoteOpen: boolean = false;

  @Output() postedNewNoteSuccessEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() postedNewNoteFailureEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() editedNoteSuccessEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() editedNoteFailureEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() deletedNoteSuccessEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() deletedNoteFailureEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() clickawayLockedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  public newNoteFormGroup: FormGroup;
  public get newNoteControls()
  {
    return this.newNoteFormGroup.controls
  }
  get newNoteSubject()
  {
    var control = this.newNoteFormGroup.get('noteSubject') as FormControl
    return control.value;
  }
  get newNoteContents()
  {
    var control = this.newNoteFormGroup.get('noteContents') as FormControl
    return control.value;
  }

  public noteFormGroup: FormGroup;
  public get noteControls()
  {
    return this.noteFormGroup.controls
  }

  public showAll: boolean = false;
  public showEditingTools: boolean = false;
  public confirmDelete: boolean = false;

  public postingNewNote: boolean = false;
  public puttingEditedNote: boolean = false;
  public deletingNote: boolean = false;

  public selectedNote: Note = new Note();

  ngOnInit(): void {
    this.noteFormGroup = this.formBuilder.group({});
    this.newNoteFormGroup = this.formBuilder.group({});

    this.addFields();
  }
  addFields()
  {
    this.newNoteFormGroup = this.formBuilder.group({
      noteSubject: ['', [Validators.required, Validators.maxLength(100)]],
      noteContents: ['', [Validators.required, Validators.maxLength(2000)]]
    });
    this.notes.forEach((note: Note) => {
      this.noteFormGroup.addControl(note.noteID, new FormControl(note.noteContents, [Validators.required, Validators.maxLength(2000)]))
    });
  }
  public setSelectedNote(note: Note)
  {
    this.selectedNote = note;
  }
  public closeCreateNewNote()
  {
    this.newNoteFormGroup.reset();
    this.createNewNoteOpen = false;
  }
  public addNewNoteToFormGroup(note: Note)
  {
    this.noteFormGroup.addControl(note.noteID, new FormControl(note.noteContents, [Validators.required, Validators.maxLength(2000)]))
  }
  getFormControlNameFromNote(note: Note)
  {
    return note.noteID
  }
  public deleteNote()
  {
    this.deletingNote = true;

    this.SantaApiDelete.deleteNote(this.selectedNote.noteID).subscribe((res) => {
      this.deletingNote = false;
      this.confirmDelete = false;
      this.showEditingTools = false;

      var controlID = this.selectedNote.noteID
      this.selectedNote = new Note();
      this.noteFormGroup.removeControl(controlID);
      this.deletedNoteSuccessEvent.emit(true);
    },err => {
      this.deletingNote = false;
      this.confirmDelete = false;
      this.showEditingTools = false;

      this.selectedNote = new Note();
      this.deletedNoteFailureEvent.emit(true);
    });
  }
  submitUpdatedNote()
  {
    this.puttingEditedNote = true;
    this.clickawayLockedEvent.emit(true);

    let response: EditNoteResponse =
    {
      noteSubject: this.selectedNote.noteSubject,
      noteContents: this.noteFormGroup.get(this.getFormControlNameFromNote(this.selectedNote)).value
    };
    this.SantaApiPut.putNote(this.selectedNote.noteID, response).subscribe((res) => {
      this.puttingEditedNote = false;
      this.selectedNote = this.mapper.mapNote(res);
      this.clickawayLockedEvent.emit(false);
      this.editedNoteSuccessEvent.emit(true);
    },err => {
      this.clickawayLockedEvent.emit(false);
      this.editedNoteFailureEvent.emit(true);
      console.group();
      console.log("Something went wrong submitting an edited note");
      console.log(err);
      console.groupEnd();
    });
  }
  public submitNewNote()
  {
    this.postingNewNote = true;
    this.clickawayLockedEvent.emit(true);

    let response: NewNoteResponse =
    {
      clientID: this.clientID,
      noteSubject: this.newNoteSubject,
      noteContents: this.newNoteContents
    };

    this.SantaApiPost.postNewClientNote(response).subscribe((res) => {
      this.postingNewNote = false;
      this.addNewNoteToFormGroup(this.mapper.mapNote(res));
      this.newNoteFormGroup.reset();
      this.clickawayLockedEvent.emit(false);
      this.postedNewNoteSuccessEvent.emit(true);
    }, err => {
      this.postingNewNote = false;
      this.clickawayLockedEvent.emit(false);
      this.postedNewNoteFailureEvent.emit(true);
      console.group();
      console.log("Something went wrong submitting a new note");
      console.log(err);
      console.groupEnd();
    });
  }
}

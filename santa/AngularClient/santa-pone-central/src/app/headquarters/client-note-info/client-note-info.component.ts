import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Client } from 'src/classes/client';

@Component({
  selector: 'app-client-note-info',
  templateUrl: './client-note-info.component.html',
  styleUrls: ['./client-note-info.component.css']
})
export class ClientNoteInfoComponent implements OnInit {

  constructor() { }

  @Input() client: Client = new Client();

  @Output() refreshEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() clickawayLockedEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  ngOnInit(): void {
  }
  public emitRefreshAction()
  {
    this.refreshEvent.emit(true);
  }
  public emitLockEvent(clickawayLocked: boolean)
  {
    this.clickawayLockedEvent.emit(clickawayLocked);
  }
}

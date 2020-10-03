import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { InputControlConstants } from 'src/app/shared/constants/InputControlConstants.enum';


@Component({
  selector: 'app-message-control-panel',
  templateUrl: './message-control-panel.component.html',
  styleUrls: ['./message-control-panel.component.css']
})
export class MessageControlPanelComponent implements OnInit {

  constructor() { }

  @Input() disabled: boolean = false;

  @Output() readAllClicked: EventEmitter<InputControlConstants> = new EventEmitter<InputControlConstants>();
  @Output() readPinnedClicked: EventEmitter<InputControlConstants> = new EventEmitter<InputControlConstants>();
  @Output() timeZoneClicked: EventEmitter<InputControlConstants> = new EventEmitter<InputControlConstants>();
  @Output() colorClicked: EventEmitter<InputControlConstants> = new EventEmitter<InputControlConstants>();


  ngOnInit(): void {
  }

  public emitReadAll()
  {
    this.readAllClicked.emit(InputControlConstants.READALL);
  }
  public emitReadPinned()
  {
    this.readPinnedClicked.emit(InputControlConstants.PINNED);
  }
  public emitTimeZone()
  {
    this.timeZoneClicked.emit(InputControlConstants.TIMEZONE);

  }
  public emitcolor()
  {
    this.colorClicked.emit(InputControlConstants.COLOR);

  }
}

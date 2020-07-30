import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-message-control-panel',
  templateUrl: './message-control-panel.component.html',
  styleUrls: ['./message-control-panel.component.css']
})
export class MessageControlPanelComponent implements OnInit {

  constructor() { }

  @Output() readAllClicked: EventEmitter<boolean> = new EventEmitter<boolean>();

  ngOnInit(): void {
  }

  public emitReadAll()
  {
    this.readAllClicked.emit(true);
  }
}

import { Component, OnInit, Input } from '@angular/core';
import { EventType } from 'src/classes/EventType';

@Component({
  selector: 'app-event-control',
  templateUrl: './event-control.component.html',
  styleUrls: ['./event-control.component.css']
})
export class EventControlComponent implements OnInit {

  constructor() { }

  @Input() allEvents: Array<EventType> = []

  ngOnInit(): void {
  }

}

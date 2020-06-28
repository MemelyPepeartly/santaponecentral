import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Message } from 'src/classes/message';

@Component({
  selector: 'app-input-control',
  templateUrl: './input-control.component.html',
  styleUrls: ['./input-control.component.css']
})
export class InputControlComponent implements OnInit {

  constructor() { }

  @Output() sendClicked: EventEmitter<Message> = new EventEmitter<Message>();

  public message = new FormControl('', Validators.required);

  ngOnInit(): void {
  }
  public emitMessage()
  {

  }
  public getErrorMessage() {
    if (this.message.hasError('required')) {
      return 'You must enter a value';
    }
    return 'Something is wrong with your message'
  }

}

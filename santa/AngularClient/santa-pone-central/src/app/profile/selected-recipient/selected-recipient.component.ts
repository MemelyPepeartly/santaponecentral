import { Component, OnInit, Input } from '@angular/core';
import { ProfileRecipient } from 'src/classes/profile';

@Component({
  selector: 'app-selected-recipient',
  templateUrl: './selected-recipient.component.html',
  styleUrls: ['./selected-recipient.component.css']
})
export class SelectedRecipientComponent implements OnInit {

  constructor() { }

  @Input() selectedRecipient: ProfileRecipient;

  ngOnInit(): void {
  }

}

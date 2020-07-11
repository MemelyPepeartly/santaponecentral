import { Component, OnInit, Input } from '@angular/core';
import { ProfileRecipient } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/Profile.service';

@Component({
  selector: 'app-selected-recipient',
  templateUrl: './selected-recipient.component.html',
  styleUrls: ['./selected-recipient.component.css']
})
export class SelectedRecipientComponent implements OnInit {

  constructor(public profileService: ProfileService) { }

  @Input() selectedRecipient: ProfileRecipient;

  ngOnInit(): void {
  }

}

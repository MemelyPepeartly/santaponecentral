import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Profile } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/Profile.service';
import { EventType } from 'src/classes/eventType';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {

  constructor(public profileService: ProfileService) { }

  @Input() profile: Profile;
  @Input() events: Array<EventType>;

  ngOnInit(): void {
  }

}

import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Profile } from 'src/classes/profile';
import { ProfileService } from 'src/app/services/Profile.service';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {

  constructor(public profileService: ProfileService) { }

  @Input() profile: Profile;

  @Output() contactRequestEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() addressRequestEvent: EventEmitter<any> = new EventEmitter<any>();


  ngOnInit(): void {
  }

  emitContactRequest()
  {
    this.contactRequestEvent.emit();
  }
  emitAddressRequest()
  {
    this.addressRequestEvent.emit();
  }

}

import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Message } from 'src/classes/message';
import { MapService, MapResponse } from 'src/app/services/mapService.service';
import { SantaApiPostService } from 'src/app/services/santaApiService.service';
import { ProfileRecipient, Profile } from 'src/classes/profile';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.css']
})
export class ContactPanelComponent implements OnInit {

  constructor(public SantaApiPost: SantaApiPostService, public responseMapper: MapResponse) { }

  @Input() chat: Array<Message>;
  @Input() selectedChatHistory: ProfileRecipient;
  @Input() profile: Profile;

  @Output() messagePosted: EventEmitter<boolean> = new EventEmitter<boolean>();

  ngOnInit(): void {
  }

  public async postTest()
  {
    var newMessageResponse = this.responseMapper.mapMessageResponse(this.selectedChatHistory, this.profile.clientID, "TEST POST")
    var res = await this.SantaApiPost.postMessage(newMessageResponse).toPromise();
    this.messagePosted.emit(true);   
  }

}

import { Injectable } from '@angular/core';
import { SantaApiGetService } from './santaApiService.service';
import { MapService } from './mapService.service';
import { MessageHistory } from 'src/classes/message';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private SantaApiGet: SantaApiGetService, private ApiMapper: MapService) { }

  private _allEventChats: BehaviorSubject<Array<MessageHistory>>= new BehaviorSubject([]);
  private _allGeneralChats: BehaviorSubject<Array<MessageHistory>>= new BehaviorSubject([]);
  private _allUnreadChats: BehaviorSubject<Array<MessageHistory>>= new BehaviorSubject([]);

  // All Event Chats
  get allEventChat()
  {
    return this._allEventChats.asObservable();
  }
  private updateAllEventChats(messageHistories: Array<MessageHistory>)
  {
    this._allEventChats.next(messageHistories);
  }

  // All General Chats
  get allGeneralChats()
  {
    return this._allGeneralChats.asObservable();
  
  }
  private updateAllGeneralChats(messageHistories: Array<MessageHistory>)
  {
    this._allGeneralChats.next(messageHistories);
  }

  // All Unread Chats
  get allUnreadChats()
  {
    return this._allUnreadChats.asObservable();
  }
  private updateAllUnreadChats(messageHistories: Array<MessageHistory>)
  {
    this._allUnreadChats.next(messageHistories);
  }

  // Gathering methods
  public async gatherEventChats()
  {

  }
  public async gatherGeneralChats()
  {

  }
  public async gatherUnreadChats()
  {
    
  }

}

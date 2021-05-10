import { Injectable } from '@angular/core';
import { MapService } from '../utility services/mapper.service';
import { MessageHistory } from 'src/classes/message';
import { BehaviorSubject } from 'rxjs';
import { MessageService } from '../api services/message.service';

@Injectable({
  providedIn: 'root'
})
export class ChatGatheringService {

  constructor(private MessageService: MessageService, private ApiMapper: MapService) { }

  private _gettingAllChats: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingAllChats()
  {
    return this._gettingAllChats.asObservable();
  }

  private _softGettingAllChats: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get softGettingAllChats()
  {
    return this._softGettingAllChats.asObservable();
  }

  private _gettingAllEventChats: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingAllEventChats()
  {
    return this._gettingAllEventChats.asObservable();
  }

  private _allChats: BehaviorSubject<Array<MessageHistory>>= new BehaviorSubject([]);
  private _allEventChats: BehaviorSubject<Array<MessageHistory>>= new BehaviorSubject([]);

  // All Chats
  get allChats()
  {
    return this._allChats.asObservable();
  }
  private updateAllChats(messageHistories: Array<MessageHistory>)
  {
    this._allChats.next(messageHistories);
  }

  // All Event Chats
  get allEventChats()
  {
    return this._allEventChats.asObservable();
  }
  private updateAllEventChats(messageHistories: Array<MessageHistory>)
  {
    this._allEventChats.next(messageHistories);
  }

  // * GATHERING METHODS * //
  public async gatherAllChats(subjectID: string, isSoftGather?: boolean)
  {
    if(!isSoftGather)
    {
      this._gettingAllChats.next(true);
    }
    if(isSoftGather)
    {
      this._softGettingAllChats.next(true);
    }

    let historyArray: Array<MessageHistory> = [];

    var data = await this.MessageService.getAllMessageHistories(subjectID).toPromise().catch(err => {
      console.log(err);
      this._gettingAllChats.next(false);
      this._softGettingAllChats.next(false);
    });

    for(let i = 0; i < data.length; i++)
    {
      historyArray.push(this.ApiMapper.mapMessageHistory(data[i]))
    }
    this.updateAllChats(historyArray);
    this._gettingAllChats.next(false);
    this._softGettingAllChats.next(false);
  }
  public clearAllChats()
  {
    this.updateAllChats([]);
    this.updateAllEventChats([]);
  }
}

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

  private _gettingAllChats: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingAllChats()
  {
    return this._gettingAllChats.asObservable();
  }

  private _gettingAllEventChats: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingAllEventChats()
  {
    return this._gettingAllEventChats.asObservable();
  }

  private _gettingSelectedHistory: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingSelectedHistory()
  {
    return this._gettingSelectedHistory.asObservable();
  }

  private _allChats: BehaviorSubject<Array<MessageHistory>>= new BehaviorSubject([]);
  private _allEventChats: BehaviorSubject<Array<MessageHistory>>= new BehaviorSubject([]);
  private _selectedHistory: BehaviorSubject<MessageHistory>= new BehaviorSubject(new MessageHistory);

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

  // Selected History
  get selectedHistory()
  {
    return this._selectedHistory.asObservable();
  }
  private updateSelectedHistory(history: MessageHistory)
  {
    this._selectedHistory.next(history);
  }


  // * GATHERING METHODS * //
  public async gatherAllChats(isSoftGather?: boolean)
  {
    if(!isSoftGather)
    {
      this._gettingAllChats.next(true);
    }
    
    let historyArray: Array<MessageHistory> = [];

    var data = await this.SantaApiGet.getAllMessageHistories().toPromise().catch(err => {console.log(err); this._gettingAllChats.next(false);});
    for(let i = 0; i < data.length; i++)
    {
      historyArray.push(this.ApiMapper.mapMessageHistory(data[i]))
    }
    this.updateAllChats(historyArray);
    this._gettingAllChats.next(false);
  }
  public async gatherEventChats(isSoftGather?: boolean)
  {
    if(!isSoftGather)
    {
      this._gettingAllEventChats.next(true);
    }
    let historyArray: Array<MessageHistory> = [];

    var data = await this.SantaApiGet.getAllEventMessageHistories().toPromise().catch(err => {console.log(err); this._gettingAllEventChats.next(false);});

    for(let i = 0; i < data.length; i++)
    {
      historyArray.push(this.ApiMapper.mapMessageHistory(data[i]))
    }
    this.updateAllEventChats(historyArray);
    this._gettingAllEventChats.next(false);
  }

  public async getSelectedHistory(clientID, relationXrefID, isSoftGather?: boolean)
  {
    if(!isSoftGather)
    {
      this._gettingSelectedHistory.next(true);
    }

    let messageHistory = new MessageHistory;
    var data = await this.SantaApiGet.getMessageHistoryByClientIDAndXrefID(clientID, relationXrefID).toPromise().catch(err => {console.log(err); this._gettingSelectedHistory.next(false);});
    messageHistory = this.ApiMapper.mapMessageHistory(data);
    this.updateSelectedHistory(messageHistory);
    this._gettingSelectedHistory.next(false);
  }
}

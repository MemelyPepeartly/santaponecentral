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

  public gettingAllChats: boolean = false;
  public gettingAllEventChats: boolean = false;


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
  public async gatherAllChats()
  {
    this.gettingAllChats = true;
    let historyArray: Array<MessageHistory> = [];

    this.SantaApiGet.getAllMessageHistories().subscribe(res => {
      for(let i = 0; i < res.length; i++)
      {
        historyArray.push(this.ApiMapper.mapMessageHistory(res[i]))
      }
      this.updateAllChats(historyArray);
      this.gettingAllChats = false;
    }, err => {console.log(err); this.gettingAllChats = false;});
  }
  public async gatherEventChats()
  {
    this.gettingAllEventChats = true;
    let historyArray: Array<MessageHistory> = [];

    this.SantaApiGet.getAllEventMessageHistories().subscribe(res => {
      for(let i = 0; i < res.length; i++)
      {
        historyArray.push(this.ApiMapper.mapMessageHistory(res[i]))
      }
      this.updateAllEventChats(historyArray);
      this.gettingAllEventChats = false;
    }, err => {console.log(err); this.gettingAllEventChats = false;}); 
  }
}

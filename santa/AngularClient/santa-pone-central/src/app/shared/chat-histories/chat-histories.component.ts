import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, OnChanges } from '@angular/core';
import { MessageHistory, ClientMeta, Message } from 'src/classes/message';
import { ProfileService } from 'src/app/services/profile.service';
import { AuthService } from 'src/app/auth/auth.service';
import { EventType } from 'src/classes/eventType';
import { BaseClient, Client } from 'src/classes/client';
import { AssignmentStatusConstants } from '../constants/AssignmentStatusConstants.enum';
import { TableVirtualScrollDataSource } from 'ng-table-virtual-scroll';

@Component({
  selector: 'app-chat-histories',
  templateUrl: './chat-histories.component.html',
  styleUrls: ['./chat-histories.component.css']
})
export class ChatHistoriesComponent implements OnInit, OnChanges {

  constructor(private profileService: ProfileService, private auth: AuthService) { }

  @Input() onProfile: boolean = false;
  @Input() histories: Array<MessageHistory>
  @Input() disabled: boolean = false;
  @Input() isRefreshingChats: boolean = false;
  @Input() viewerClient: BaseClient = new BaseClient();

  columns: string[] = ["sender", "assignment", "event", "status", "contact"];

  public isAdmin: boolean = false;
  dataSource = new TableVirtualScrollDataSource();

  @Output() chatSelectedEvent: EventEmitter<MessageHistory> = new EventEmitter<MessageHistory>();
  @Output() agentSelectedEvent: EventEmitter<{meta: ClientMeta, event: EventType}> = new EventEmitter<{meta: ClientMeta, event: EventType}>();


  public ngOnInit(): void {
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
    this.dataSource = new TableVirtualScrollDataSource(this.histories);
  }
  public ngOnChanges(changes: SimpleChanges): void {
    if (changes?.yuleLogs) {
      this.dataSource = new TableVirtualScrollDataSource(this.histories);
    }
  }
  public emitSelectedHistory(history: MessageHistory)
  {
    this.chatSelectedEvent.emit(history);
  }
  public emitAgentSelected(historyMeta: ClientMeta, historyEvent: EventType)
  {
    this.agentSelectedEvent.emit({meta: historyMeta, event: historyEvent});
  }
  public isCompleted(history: MessageHistory) : boolean
  {
    return history.assignmentStatus.assignmentStatusName == AssignmentStatusConstants.COMPLETED;
  }
  public isDisabled(history: MessageHistory) : boolean
  {
    return this.disabled || (this.isAdmin && this.onProfile) || this.viewerClient.clientID == history.conversationClient.clientID
  }
}

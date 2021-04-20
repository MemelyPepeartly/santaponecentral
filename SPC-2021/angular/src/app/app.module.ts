import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CustomMaterialModule } from './core/material.module';

import { ChatHistoriesComponent } from './shared/chat-histories/chat-histories.component';
import { AssignmentStatusControllerComponent } from './shared/assignment-status-controller/assignment-status-controller.component';
import { ContactPanelComponent } from './shared/contact-panel/contact-panel.component';
import { InputControlComponent } from './shared/input-control/input-control.component';
import { LoadingSpinComponent } from './shared/loading-spin/loading-spin.component';
import { SmallLoadingSpinComponent } from './shared/small-loading-spin/small-loading-spin.component';
import { ResponseListComponent } from './shared/response-list/response-list.component';
import { MissionBoardTableComponent } from './shared/mission-board-table/mission-board-table.component';
import { MessageControlPanelComponent } from './shared/message-control-panel/message-control-panel.component';
import { ChatComponent } from './shared/chat/chat.component';

@NgModule({
  declarations: [
    AppComponent,
    ChatHistoriesComponent,
    AssignmentStatusControllerComponent,
    ContactPanelComponent,
    InputControlComponent,
    LoadingSpinComponent,
    SmallLoadingSpinComponent,
    ResponseListComponent,
    MissionBoardTableComponent,
    MessageControlPanelComponent,
    ChatComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CustomMaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

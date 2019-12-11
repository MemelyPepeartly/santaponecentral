//Necessary
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

//Extras
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CustomMaterialModule } from './core/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

//Componenents
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { SantaponecontrolComponent } from './santaponecontrol/santaponecontrol.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SantaponenavComponent } from './navbar/santaponenav/santaponenav.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { LoginComponent } from './splash/login/login.component';
import { WhatisInformationComponent } from './splash/whatis-information/whatis-information.component';
import { PrivacyInformationComponent } from './splash/privacy-information/privacy-information.component';
import { WhoisInformationComponent } from './splash/whois-information/whois-information.component';
import { NologinInformationComponent } from './splash/nologin-information/nologin-information.component';
import { FaqInformationComponent } from './splash/faq-information/faq-information.component';
import { DefaultnavComponent } from './navbar/defaultnav/defaultnav.component';
import { ContactComponent } from './contact/contact.component';
import { SignupComponent } from './signup/signup.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProfileComponent,
    SantaponecontrolComponent,
    NotificationsComponent,
    NavbarComponent,
    SantaponenavComponent,
    UsernavComponent,
    LoginComponent,
    WhatisInformationComponent,
    PrivacyInformationComponent,
    WhoisInformationComponent,
    NologinInformationComponent,
    FaqInformationComponent,
    DefaultnavComponent,
    ContactComponent,
    SignupComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CustomMaterialModule,
    FormsModule,
    AppRoutingModule,
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

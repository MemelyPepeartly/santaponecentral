import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SplashComponent } from './splash/splash.component';
import { ProfileComponent } from './profile/profile.component';
import { SantaponecontrolComponent } from './santaponecontrol/santaponecontrol.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { MatMenuModule } from '@angular/material/menu';
import { NavbarComponent } from './navbar/navbar.component';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Routes, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { SantaponenavComponent } from './navbar/santaponenav/santaponenav.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoginComponent } from './splash/login/login.component';
import { SignupComponent } from './splash/signup/signup.component';
import { InformationComponent } from './splash/information/information.component';

const appRoutes: Routes = [
  { path: 'profile', component: ProfileComponent },
  {
    path: 'splash',
    component: SplashComponent
  },
  { path: '',
    redirectTo: '/splash',
    pathMatch: 'full'
  }
];
@NgModule({
  declarations: [
    AppComponent,
    SplashComponent,
    ProfileComponent,
    SantaponecontrolComponent,
    NotificationsComponent,
    NavbarComponent,
    SantaponenavComponent,
    UsernavComponent,
    LoginComponent,
    SignupComponent,
    InformationComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatMenuModule,
    MatIconModule,
    MatToolbarModule,
    MatButtonModule,
    MatTabsModule,
    MatCardModule,
    MatFormFieldModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    )
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

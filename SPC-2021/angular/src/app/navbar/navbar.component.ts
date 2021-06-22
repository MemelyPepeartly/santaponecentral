import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { RoleConstants } from '../shared/constants/roleConstants.enum';
import { User } from '@auth0/auth0-spa-js';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public auth: AuthService) { }

  public get navType() : string
  {
    let type: string = "default";
    if(this.isAdmin)
    {
      type = "admin"
    }
    else if (!this.isAdmin && this.isAuthenticated)
    {
      type = "user"
    }
    return type
  }

  profile: User;

  public isAdmin: boolean = false;
  public isAuthenticated: boolean = false;

  ngOnInit() {
    this.auth.user$.subscribe((userInfo) => {
      this.profile = userInfo;
      if(this.profile["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"].includes(RoleConstants.ADMIN))
      {
        this.isAdmin = true;
      }
      else if (this.profile["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"].includes(RoleConstants.USER))
      {
        this.isAdmin = false;
      }
      else if (this.profile["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"].includes(RoleConstants.DEVELOPER))
      {
        this.isAdmin = true;
      }
      console.log(this.profile);
      
    });
  }
}

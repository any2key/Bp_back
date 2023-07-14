import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Router } from '@angular/router';
import { TokenService } from './services/token.service';
import { UiService } from './services/ui.service';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']

})
export class AppComponent {
  title = 'app';
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  constructor(private userSerive: UserService, private tokenService: TokenService, private router: Router, private ui: UiService) {

  }

  get Role() {
    return this.tokenService.getSession()?.userRole;
  }

  get User() {
    return this.tokenService.getSession()?.login;
  }

  logout() {
    this.ui.confirmation().subscribe(res => {
      if (res) {

        this.tokenService.logout();
        this.router.navigate(['home']);
      }
    });
  }
  route(path: string) {
    this.router.navigate([path]);
  }
}

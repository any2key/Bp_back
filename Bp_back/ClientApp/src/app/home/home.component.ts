import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../services/token.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  constructor(private router: Router, private token: TokenService, private user: UserService) { }

  ngOnInit(): void {
    if (this.token.getSession()?.userRole == "admin")
      this.router.navigate(['admin/dashboard']);
    else if (this.token.getSession()?.userRole == "user")
      this.router.navigate(['user/lk']);
    else if (this.token.getSession()?.userRole == "smuser")
      this.router.navigate(['smuser/lk']);
    else
      this.router.navigate(['login']);
  }


}

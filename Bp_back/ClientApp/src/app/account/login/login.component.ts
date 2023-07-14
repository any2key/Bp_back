import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginRequest } from '../../model';
import { TokenService } from '../../services/token.service';
import { UiService } from '../../services/ui.service';
import { UserService } from '../../services/user.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private user: UserService, private router: Router, private ui: UiService, private token: TokenService) { }

  ngOnInit(): void {
  }

  login = new FormGroup(
    {
      Login: new FormControl(''),
      Password: new FormControl('')
    });

  keyUpFunction(res: any) {
    console.log(res);
  }
  loginMe() {
    this.user.login(new LoginRequest(this.login.get('Login')?.value, this.login.get('Password')?.value)).subscribe(res => {
      if (!res.isOk)
        this.ui.show(res.message);
      else {
        this.token.saveSession(res.data);
        this.router.navigate(['home']);
      };
    });
  }
}

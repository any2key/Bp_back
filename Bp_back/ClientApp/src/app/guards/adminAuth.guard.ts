import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { TokenService } from '../services/token.service';
const LS_ROLE = 'role';

@Injectable({
  providedIn: 'root'
})


export class AdminAuthGuard implements CanActivate {
  constructor(private router: Router, private token: TokenService) { }

  canActivate() {
    if (this.token.getSession()?.userRole != "admin") {
      this.router.navigate(['/forbidden']);
      return false;
    }
    else return true;
  }
}

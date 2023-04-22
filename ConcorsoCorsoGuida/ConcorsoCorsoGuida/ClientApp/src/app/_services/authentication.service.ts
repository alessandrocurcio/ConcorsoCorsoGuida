import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { User } from '../_models';
import { Proxy } from '../_helpers';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthenticationService
{
  constructor(private http: HttpClient, private proxy: Proxy, private router: Router) { }

  login(username: string, password: string)
  {
    return this.proxy.call<User>('user', 'authenticate', { username, password }).pipe(map(user =>
    {
      if (!user) return null;
      localStorage.setItem('currentUser', JSON.stringify({ token: user.token }));
      return user;
    }));
  }

  logout()
  {
    localStorage.removeItem('currentUser');
  }

  public currentUser(): Observable<User>
  {
    let currUser: string = "{}";
    if (localStorage.getItem('currentUser') !== null)
      currUser = localStorage.getItem('currentUser')!;
    let cToken = JSON.parse(currUser);
    if (!cToken) cToken = { token: "" };
    return this.proxy.call<User>('user', 'GetUserInfoByToken', { token: cToken.token }).pipe(map(user =>
    {
      return user;
    })
      , catchError(err => {
      console.log(err);
      this.logout();
        this.router.navigate(['/login']);
        return throwError(err);
    }));
  }
}

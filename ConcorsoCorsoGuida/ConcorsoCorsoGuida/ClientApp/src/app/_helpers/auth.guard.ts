import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { map } from 'rxjs/operators';

import { AuthenticationService } from '../_services';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate
{
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
  {
    return this.authenticationService.currentUser().pipe(map((currentUser) =>
    {
      if (currentUser && currentUser.id !== 0)
      {
        //Check if the pages are visibile only for admin
        if (state.url === '/registration')
        {
          if (currentUser.role === 1)
            return true;
        }
        else
          return true;
      }

      // not logged in so redirect to login page with the return url
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }));
  }
}

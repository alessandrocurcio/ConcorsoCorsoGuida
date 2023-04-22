import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../_models';
import { AuthenticationService } from '../_services';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  currentUser: User = new User();

  ngOnInit() {
    this.authenticationService.currentUser().subscribe(x => this.currentUser = x);
  }

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}

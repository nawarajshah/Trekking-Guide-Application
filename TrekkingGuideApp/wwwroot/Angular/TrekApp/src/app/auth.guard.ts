import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from './service/auth.service';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
  constructor(private authService: AuthService, private router: Router) {}

  // check if the current user is authenticated and if their roles match the expected roles (provided via route data)
  canActivate(
    route: ActivatedRouteSnapshot
  ): Observable<boolean | UrlTree> {
    // expected roles passed via route data, e.g. data: {roles: ['Admin', 'SuperAdmin']}
    const expectedRoles: string[] = route.data['roles'];
    return this.authService.getUserRoles().pipe(
      map((roles: string[]) => {
        // if no roles returned, assume not logged in
        if (!roles || roles.length === 0) {
          return this.router.createUrlTree(['/Account/Login']);
        }
        // if expected roles are specified, ensure the user has atleast one
        if (expectedRoles && expectedRoles.length > 0) {
          const hasRole = roles.some(r => expectedRoles.includes(r));
          if (!hasRole) {
            // if not allowed, redirect to home (or an access denied page)
            return this.router.createUrlTree(['/']);
          }
        }
        return true;
      }),
      catchError(() => {
        return of(this.router.createUrlTree(['/Account/Login']));
      })
    );
  }
}

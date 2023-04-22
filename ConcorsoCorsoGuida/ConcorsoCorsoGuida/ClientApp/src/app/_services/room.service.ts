import { Injectable, PipeTransform } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Proxy } from '../_helpers';
import { Room, User } from '../_models';
import { SortColumn, SortDirection } from '../_helpers/sortable.directive';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';


interface SearchResult
{
  rooms: Room[];
  total: number;
}

interface State
{
  page: number;
  pageSize: number;
  searchTerm: string;
  sortColumn: SortColumn;
  sortDirection: SortDirection;
}

@Injectable({ providedIn: 'root' })
export class RoomService
{

  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _rooms$ = new BehaviorSubject<Room[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private _rooms: Room[] = [];

  private _state: State = {
    page: 1,
    pageSize: 4,
    searchTerm: '',
    sortColumn: '',
    sortDirection: ''
  };

  constructor(private http: HttpClient, private proxy: Proxy)
  {
  }

  getAll(): Observable<Room[]>
  {
    return this.http.get<Room[]>(`${window.location.origin}/room`);
  }

  GetMyRooms(inputUser: User): Observable<Room[]>
  {
    return this.proxy.call<Room[]>("room","getmyrooms", inputUser);
  }

  save(inputReservation: Room): Observable<Room>
  {
    return this.proxy.call<Room>("room","save", inputReservation);
  }

  delete(inputReservation: Room): Observable<boolean>
  {
    return this.proxy.call<boolean>("room","delete", inputReservation);
  }


  get rooms$() { return this._rooms$.asObservable(); }
  get total$() { return this._total$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }
  get page() { return this._state.page; }
  get pageSize() { return this._state.pageSize; }
  get searchTerm() { return this._state.searchTerm; }

  set page(page: number) { this._set({ page }); }
  set pageSize(pageSize: number) { this._set({ pageSize }); }
  set searchTerm(searchTerm: string) { this._set({ searchTerm }); }
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }

  private _set(patch: Partial<State>)
  {
    Object.assign(this._state, patch);
    this._search$.next();
  }


}

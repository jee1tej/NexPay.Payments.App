import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';

import { ITransaction } from './Transaction';

@Injectable()
export class TransactionService {
    private baseUrl = 'api/Transactions/';

    constructor(private http: Http) { }

    saveTransaction(product: ITransaction): Observable<ITransaction> {
        let headers = new Headers({ 'Content-Type': 'application/json' });

        let options = new RequestOptions({ headers: headers });

        return this.createTransaction(product, options);
    }

    private createTransaction(transaction: ITransaction, options: RequestOptions): Observable<ITransaction> {
        return this.http.post(this.baseUrl + transaction.accountNumber, JSON.stringify(transaction), options)
            .map(this.extractData)
            .do(data => console.log('createProduct: ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private extractData(response: Response) {
        let body = response.json();
        return body.data || {};
    }

    private handleError(error: Response): Observable<any> {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }

}

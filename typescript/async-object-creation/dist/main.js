/**
 * Asynchronous object creation tool
 * The base AsyncObject can be extended to handle asynchronous creation
 * without repeating code and while preserving code completion for the constructor
 */
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class Utilities {
    /**
     * Waits for [delay] and resolves promise with a new value
     * @param delay
     */
    static fetchWithDelay(delay) {
        return __awaiter(this, void 0, void 0, function* () {
            // use to simulate delay in fetching data
            let value = 'fetched';
            // return Promise.reject(new Error("can't create timer"));
            return new Promise((resolve) => {
                console.log("started fetching data at " + new Date().toLocaleString());
                setTimeout(() => {
                    console.log("finished fetching data at " + new Date().toLocaleString());
                    resolve(value);
                }, delay);
            });
        });
    }
}
class AsyncObject {
    // set parameters in child constructor
    constructor() {
        // initialize properties in child constructor
    }
    // instantiate the object and return it in a promise
    static createAsync(...args) {
        return __awaiter(this, void 0, void 0, function* () {
            // use: [Child].createAsync(args)
            // the arguments are the same as those provided in the child constructor
            // create object instance
            let instance = new this(...args);
            // call asyncConstructor on instance and wrap it in a promise
            let promise = instance.asyncConstruction(args).then((result) => {
                // store constructor output in new instance
                instance.constructorOutput = result;
                console.log("constructor output: " + result);
                return instance;
            }).catch((err) => {
                console.error(err.message);
                throw new Error("async constructor failed");
            });
            // return the promised instance
            return promise;
        });
    }
}
// test
class TestAsyncChild extends AsyncObject {
    // ask for parameters 
    constructor(param) {
        super();
        this.classProperty = "original";
        this.fetchedProperty = "default"; // to simulate a delay in fetching data at construction
        console.log('default class property value: ' + this.classProperty);
        console.log('default fetched property value instance: ' + this.fetchedProperty);
        // initialize properties in constructor
        this.classProperty = param;
    }
    // implement asynchronous constructor
    asyncConstruction() {
        return __awaiter(this, void 0, void 0, function* () {
            // simulate fetching data with a delay
            let fetchResult = Utilities.fetchWithDelay(2000).then((result) => {
                this.fetchedProperty = result;
                // optionally return a success flag for the construction (or anything else)
                return "success";
            }).catch((err) => {
                console.error(err.message);
                throw new Error("coulnd't fetch result");
            });
            return fetchResult;
        });
    }
}
// create object and manipulate it once instantiation is done
// note that parameters completion is maintained for the child class in the createAsync method
let asyncObject = TestAsyncChild.createAsync("modified");
asyncObject.then((result) => {
    console.log("instance class property value: " + result.classProperty);
    console.log("instance fetched property value: " + result.fetchedProperty);
}).catch((err) => {
    console.error(err.message);
    throw new Error("couldn't initialize object");
});
//# sourceMappingURL=main.js.map
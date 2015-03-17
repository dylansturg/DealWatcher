//
//  Seller.h
//  DealWatcher
//
//  Created by Dylan Sturgeon on 3/17/15.
//  Copyright (c) 2015 dylansturg. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class ProductPrice;

@interface Seller : NSManagedObject

@property (nonatomic, retain) NSNumber * sellerId;
@property (nonatomic, retain) NSString * name;
@property (nonatomic, retain) NSString * website;
@property (nonatomic, retain) ProductPrice *prices;

@end

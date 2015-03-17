//
//  ProductPrice.h
//  DealWatcher
//
//  Created by Dylan Sturgeon on 3/17/15.
//  Copyright (c) 2015 dylansturg. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Product, Seller;

@interface ProductPrice : NSManagedObject

@property (nonatomic, retain) NSNumber * priceId;
@property (nonatomic, retain) NSString * gathered;
@property (nonatomic, retain) NSString * locationUrl;
@property (nonatomic, retain) NSDecimalNumber * price;
@property (nonatomic, retain) NSNumber * productId;
@property (nonatomic, retain) NSNumber * sellerId;
@property (nonatomic, retain) Product *product;
@property (nonatomic, retain) Seller *seller;

@end

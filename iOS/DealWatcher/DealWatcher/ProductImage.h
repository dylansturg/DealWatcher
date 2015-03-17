//
//  ProductImage.h
//  DealWatcher
//
//  Created by Dylan Sturgeon on 3/17/15.
//  Copyright (c) 2015 dylansturg. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Product;

@interface ProductImage : NSManagedObject

@property (nonatomic, retain) NSNumber * imageId;
@property (nonatomic, retain) NSString * url;
@property (nonatomic, retain) NSNumber * productId;
@property (nonatomic, retain) Product *product;

@end

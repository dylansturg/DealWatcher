//
//  ProductCode.h
//  DealWatcher
//
//  Created by Dylan Sturgeon on 3/17/15.
//  Copyright (c) 2015 dylansturg. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Product;

@interface ProductCode : NSManagedObject

@property (nonatomic, retain) NSNumber * codeId;
@property (nonatomic, retain) NSNumber * typeId;
@property (nonatomic, retain) NSString * type;
@property (nonatomic, retain) NSString * code;
@property (nonatomic, retain) NSNumber * productId;
@property (nonatomic, retain) Product *product;

@end

//
//  AppDelegate.m
//  DealWatcher
//
//  Created by Dylan Sturgeon on 3/16/15.
//  Copyright (c) 2015 dylansturg. All rights reserved.
//

#import "AppDelegate.h"
#import <RestKit/CoreData.h>
#import <RestKit/RestKit.h>

@interface AppDelegate ()

@end

@implementation AppDelegate


- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    // Override point for customization after application launch.
    
    NSURL *baseUrl = [NSURL URLWithString:@"https://dealwatcherservice.azurewebsites.net"];
    RKObjectManager *objectManager = [RKObjectManager managerWithBaseURL:baseUrl];
    
    // Initialize managed object model from bundle
    NSManagedObjectModel *managedObjectModel = [NSManagedObjectModel mergedModelFromBundles:nil];
    // Initialize managed object store
    RKManagedObjectStore *managedObjectStore = [[RKManagedObjectStore alloc] initWithManagedObjectModel:managedObjectModel];
    objectManager.managedObjectStore = managedObjectStore;
    
    // Complete Core Data stack initialization
    [managedObjectStore createPersistentStoreCoordinator];
    NSString *storePath = [RKApplicationDataDirectory() stringByAppendingPathComponent:@"DealDB.sqlite"];
    NSError *error;
    NSPersistentStore *persistentStore = [managedObjectStore addSQLitePersistentStoreAtPath:storePath fromSeedDatabaseAtPath:nil withConfiguration:nil options:nil error:&error];
    NSAssert(persistentStore, @"Failed to add persistent store with error: %@", error);
    
    // Create the managed object contexts
    [managedObjectStore createManagedObjectContexts];
    
    // Configure a managed object cache to ensure we do not create duplicate objects
    managedObjectStore.managedObjectCache = [[RKInMemoryManagedObjectCache alloc] initWithManagedObjectContext:managedObjectStore.persistentStoreManagedObjectContext];
    
    // Entity Mapping
    RKEntityMapping *imagesMapping = [RKEntityMapping mappingForEntityForName:@"ProductImage" inManagedObjectStore:managedObjectStore];
    imagesMapping.identificationAttributes = @[@"imageId"];
    [imagesMapping addAttributeMappingsFromDictionary:@{
                                                        @"Id":@"imageId",
                                                        @"ProductId":@"productId",
                                                        @"Url":@"Url"
                                                        }];
    
    RKEntityMapping *pricesMapping = [RKEntityMapping mappingForEntityForName:@"ProductPrice" inManagedObjectStore:managedObjectStore];
    pricesMapping.identificationAttributes = @[@"priceId"];
    [pricesMapping addAttributeMappingsFromDictionary:@{
                                                        @"Id":@"priceId",
                                                        @"ProductId":@"productId",
                                                        @"SellerId":@"sellerId",
                                                        @"Gathered":@"gathered",
                                                        @"LocationUrl":@"locationUrl",
                                                        @"Price":@"price"
                                                        }];
    
    RKEntityMapping *codesMapping = [RKEntityMapping mappingForEntityForName:@"ProductCode" inManagedObjectStore:managedObjectStore];
    codesMapping.identificationAttributes = @[@"codeId"];
    [codesMapping addAttributeMappingsFromDictionary:@{
                                                       @"Id":@"codeId",
                                                       @"TypeId":@"typeId",
                                                       @"Type":@"type",
                                                       @"ProductId":@"productId",
                                                       @"Code":@"code"
                                                       }];
    
    RKEntityMapping *productsMapping = [RKEntityMapping mappingForEntityForName:@"Product" inManagedObjectStore:managedObjectStore];
    productsMapping.identificationAttributes = @[@"productId"];
    [productsMapping addAttributeMappingsFromDictionary:@{
                                                          @"Id":@"productId",
                                                          @"DisplayName":@"displayName"
                                                          }];
    [productsMapping addPropertyMapping:[RKRelationshipMapping relationshipMappingFromKeyPath:@"ProductImages" toKeyPath:@"productImages" withMapping:imagesMapping]];
    [productsMapping addPropertyMapping:[RKRelationshipMapping relationshipMappingFromKeyPath:@"ProductPrices" toKeyPath:@"productPrices" withMapping:pricesMapping]];
    [productsMapping addPropertyMapping:[RKRelationshipMapping relationshipMappingFromKeyPath:@"ProductCodes" toKeyPath:@"productCodes" withMapping:codesMapping]];
    
    RKResponseDescriptor *productsDescriptor = [RKResponseDescriptor responseDescriptorWithMapping:productsMapping method:RKRequestMethodGET pathPattern:@"/api/Products" keyPath:nil statusCodes:RKStatusCodeIndexSetForClass(RKStatusCodeClassSuccessful)];
    [objectManager addResponseDescriptor:productsDescriptor];
    
    
    [AFNetworkActivityIndicatorManager sharedManager].enabled = YES;
    
    return YES;
}

- (void)applicationWillResignActive:(UIApplication *)application {
    // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
    // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
}

- (void)applicationDidEnterBackground:(UIApplication *)application {
    // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
    // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
}

- (void)applicationWillEnterForeground:(UIApplication *)application {
    // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
}

- (void)applicationDidBecomeActive:(UIApplication *)application {
    // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
}

- (void)applicationWillTerminate:(UIApplication *)application {
    // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
}

@end

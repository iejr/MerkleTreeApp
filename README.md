# Merkle Tree Demo
This is a merkle tree demo project, contains a merkle tree library, console application and webapi application

## Project Structure
 /
 ├── MerkleTree/              # Merkle Tree Library, include the core data structures and algorithms
 ├── MerkleTreeTest/          # Merkle Tree Library Unit Test
 ├── MerkleTreeMain/          # Console Application for Merkle Tree
 ├── MerkleTreeApi/           # Webapi of Merkle Tree
 ├── MerkleTreeApiTest/       # Merkle Tree Webapi Unit Tests

## MerkleTree
 
 This is a C# library that contains the core data structure and methods.

 To build the library, run `dotnet build`

### Data Structure
 Here the underlying data structure is a binary tree. But each node is unlike the classical binary tree node which contains left child pointer and right child pointer, instead, a pointer to parent and another pointer to sibling. This is because the query (merkle proof) over merkle tree goes from bottom to top, thus, this data structure is a better fit.

### Tagged Hash
 Each node contains a value of hashing. For leaf node it's the hash of input payload; Branch node hash is the hash of the concatenate of its children's hash. All hash function here use tagged hash to reduce the conflict with other hash purpose's possibility. Leaf hash tag can be different from branch hash tag.

### Build Merkle Tree
 When building the merkle tree from a list of payloads, it follows level order traversal from bottom to top as well. The bottom level is also known as leaf node, which contains hash of one payload. All leaf nodes created are put into a queue, then the front two node are paired up to generate their common parent, which is then pushed into the back of the queue, until there is only one node left in the queue, which is the final root. 

 When the input contains only odd number of payloads, the last one will be duplicated so that they are all paired without error. Also, during the level order traversal there might be odd number of nodes in one level, the last one will be re-pushed into the queue for being further processed in the upper level.

### Merkle Proof 
 There is a hash table recording the map between input payload to its leaf node, for fast lookup during merkle proof. 

 Once the leaf node is identified, we'll go up until root with all siblings' hash put into the result, which is so called a merkle proof.

## MerkleTreeTest

 Contains all unit test of the merkle tree library. 

 To run the test, run `dotnet build && dotnet test`

## MerkleTreeMain

 This is a console application to run a merkle tree library. 

 To run it, `dotnet build && dotnet run`

## MerkleTreeApi

 This is a webapi interface of the merkle tree library. By default, it contains a demo of using merkle tree to proof of reserve of user's balance

 To run the demo, `dotnet build && dotnet run`, then a local server will run and listen at the port specified from the log output

 There are two exmaple api endpoint in this user balance demo: create & get balance. Both are POST method, and all requests and response are in JSON format.

### Create
 The api accept three arguments, a list of pair of (user id, balance) for building the tree, leaf tag and branch tag for the hashing purpose. Both user id and balance are in integer format. The merkle tree would be built against serialized string of (user id, balance). Once the tree is built, the api will return root hash for client's record. The list argument is required while leaf tag and branch tag are optional with default values

 The web service utilized a user state service to store the merkle tree built and a map between user id to a User object while contains user id and balance. This is a singleton service.

### Get Balance
 The api accept the user id in integer format. If the user id matches the what recorded previously in the user state service, the api would then proceed to perform a merkle proof call to the libaray, finally return:
   - User balance in integer format
   - A list of pair (node hash, left or right child) from the closest to the leaf node's sibling to the top
 to the client. Client can verify by
   - 1. Compute the leaf node hash with the user id, user balance and leaf hash tag
   - 2. Loop the the returned list to verify each branch hash with branch hash tag
   - 3. Finally verify the result matches the root hash which already returned to client when calling the "Create" api

## MerkleTreeApiTest
 This contains all tests of the webapi project.

 To run the test, `dotnet build && dotnet test`

# Some Future Work
- Re-implement merkle tree data structure by heap-like array
- Add failure case handler 
- TODO